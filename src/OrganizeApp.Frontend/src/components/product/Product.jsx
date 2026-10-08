import { useEffect, useState } from "react" // useCallback
import ProductForm from "./ProductForm"
import ProductList from "./ProductList"
import { useForm } from "react-hook-form"
import toast from "react-hot-toast"
import axios from "axios"
import getErrorMessage from "../../utils/errorHelper"

const BASE_URL = `${import.meta.env.VITE_BASE_API_URL}/products`;
const CATEGORIES_URL = `${import.meta.env.VITE_BASE_API_URL}/categories`;

// CreateProductDTO/UpdateProductDTO: solo name, description, imageUrl y price.
// El stock empieza en 0 y se gestiona con los endpoints increase/decrease.
const defaultFormValues = {
    id: 0,
    name: '',
    description: '',
    imageUrl: '',
    price: ''
}

function Product() {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [editData, setEditData] = useState(null);

    const methods = useForm({
        defaultValues: defaultFormValues
    });

    useEffect(() => {
        methods.reset(editData ?? defaultFormValues);
    }, [editData, methods]);

    const loadProducts = async () => {
        try {
            const productsData = (await axios.get(BASE_URL)).data;
            setProducts(productsData);
        } catch (error) {
            console.log(error);
            let txt = getErrorMessage(error);
            toast.error(txt);
        }
    };
    // const loadProducts = useCallback(async () => {
    //     setLoading(true);
    //     try {
    //         const productsData = (await axios.get(BASE_URL)).data;
    //         setProducts(productsData);
    //     } catch (error) {
    //         console.log(error);
    //         toast.error("Error loading products!");
    //     }
    //     finally {
    //         setLoading(false);
    //     }
    // }, []);

    // useEffect(() => {
    //     loadProducts();
    // }, [loadProducts]);

    useEffect(() => {
        const loadInitialProducts = async () => {
            setLoading(true);

            try {
                const productsData = (await axios.get(BASE_URL)).data;
                setProducts(productsData);
            } catch (error) {
                console.log(error);
                let txt = getErrorMessage(error);
                toast.error(txt);
            }
            finally {
                setLoading(false);
            }
        };

        loadInitialProducts();
    }, []);

    useEffect(() => {
        const loadCategories = async () => {
            try {
                const categoriesData = (await axios.get(CATEGORIES_URL)).data;
                setCategories(categoriesData);
            } catch (error) {
                console.log(error);
                //toast.error("Error loading categories!");
                let txt =  getErrorMessage(error); 
                toast.error(txt);
            }
        };
        loadCategories();
    }, []);

    // reset
    const handleFormReset = () => {
        setEditData(null);
        methods.reset(defaultFormValues);
    }

    const handleFormSubmit = async (product) => {
        setLoading(true);
        // El backend no acepta id ni stock en create/update
        const payload = {
            name: product.name,
            description: product.description || null,
            imageUrl: product.imageUrl,
            price: Number(product.price)
        };
        try {
            if (product.id <= 0) {
                // POST /api/products — devuelve el DTO enviado (sin id real),
                // por eso recargamos la lista desde el servidor
                await axios.post(BASE_URL, payload);
                await loadProducts();
            }
            else {
                // PATCH /api/products/{id}
                await axios.patch(`${BASE_URL}/${product.id}`, payload);
                setProducts((previousProducts) =>
                    previousProducts.map(p => p.id === product.id ? { ...p, ...payload } : p));
            }
            handleFormReset();
            toast.success("Saved successfully!");
        } catch (error) {
            console.log(error);
            let txt = getErrorMessage(error);
            toast.error(txt);
        }
        finally {
            setLoading(false);
        }
    }

    const handleProductEdit = (product) => {
        setEditData(product);
    }

    const handleProductDelete = async (product) => {
        if (!confirm(`Are you sure to delete a product : ${product.name} ${product.description ?? ''}`)) return;
        setLoading(true);
        try {
            await axios.delete(`${BASE_URL}/${product.id}`);
            setProducts((previousProducts) => previousProducts.filter(p => p.id !== product.id));
            toast.success("Deleted successfully!");
        } catch (error) {
            console.log(error);
            let txt = getErrorMessage(error);
            toast.error(txt);
        }
        finally {
            setLoading(false);
        }
    }

    // POST /api/products/{id}/stock/increase | /decrease  (body: { quantity })
    const handleStockChange = async (productId, quantity, action) => {
        setLoading(true);
        try {
            await axios.post(`${BASE_URL}/${productId}/stock/${action}`, { quantity });
            await loadProducts();
            toast.success(`Stock successfully ${action === 'increase' ? 'increased' : 'decreased'}!`);
        } catch (error) {
            console.log(error);
            let txt = getErrorMessage(error);
            toast.error(txt);
        }
        finally {
            setLoading(false);
        }
    }

    // POST /api/products/{id}/activate | /deactivate
    const handleToggleAvailability = async (product) => {
        setLoading(true);
        const action = product.isAvailable ? 'deactivate' : 'activate';
        try {
            await axios.post(`${BASE_URL}/${product.id}/${action}`);
            setProducts((previousProducts) =>
                previousProducts.map(p => p.id === product.id ? { ...p, isAvailable: !product.isAvailable } : p));
            toast.success(`Product successfully ${action}d!`);
        } catch (error) {
            console.log(error);
            let txt = getErrorMessage(error);
            toast.error(txt);
        }
        finally {
            setLoading(false);
        }
    }

    // POST /api/products/{productId}/categories/{categoryId}
    const handleAssignCategory = async (productId, categoryId) => {
        setLoading(true);
        try {
            await axios.post(`${BASE_URL}/${productId}/categories/${categoryId}`);
            await loadProducts();
            toast.success("Category successfully assigned!");
        } catch (error) {
            console.log(error);
            let txt = getErrorMessage(error);
            toast.error(txt);
        }
        finally {
            setLoading(false);
        }
    }

    // DELETE /api/products/{productId}/categories/{categoryId}
    const handleRemoveCategory = async (productId, categoryId) => {
        setLoading(true);
        try {
            await axios.delete(`${BASE_URL}/${productId}/categories/${categoryId}`);
            await loadProducts();
            toast.success("Category successfully removed!");
        } catch (error) {
            console.log(error);
            let txt = getErrorMessage(error);
            toast.error(txt);
        }
        finally {
            setLoading(false);
        }
    }

    return (
        <div className="min-h-screen bg-gray-50 py-8">
            <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 space-y-6">
                <div className="text-center mb-8">
                    <h1 className="text-3xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
                        Product Management
                    </h1>
                    {loading && <p>Loading...</p>}
                </div>

                <ProductForm
                    methods={methods}
                    onFormSubmit={handleFormSubmit}
                    onFormReset={handleFormReset}
                    isEditing={editData !== null}
                />
                <ProductList
                    productsList={products}
                    categories={categories}
                    onProductEdit={handleProductEdit}
                    onProductDelete={handleProductDelete}
                    onStockChange={handleStockChange}
                    onToggleAvailability={handleToggleAvailability}
                    onAssignCategory={handleAssignCategory}
                    onRemoveCategory={handleRemoveCategory}
                />
            </div>
        </div>
    )
}

export default Product