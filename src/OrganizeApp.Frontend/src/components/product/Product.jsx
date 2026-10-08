import { useCallback, useEffect, useState } from "react"
import ProductForm from "./ProductForm"
import ProductList from "./ProductList"
import { useForm } from "react-hook-form"
import toast from "react-hot-toast"
import {
    getProducts,
    createProduct,
    updateProduct,
    deleteProduct,
    increaseStock,
    decreaseStock,
    activateProduct,
    deactivateProduct,
    assignCategory,
    removeCategory
} from "../../services/productService"
import { getCategories } from "../../services/categoryService"
import { getErrorMessage } from "../../utils/errorHelper"

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

    const loadProducts = useCallback(async () => {
        try {
            const { data } = await getProducts();
            setProducts(data);
        } catch (error) {
            console.error(error);
            toast.error(getErrorMessage(error));
        }
    }, []);


    useEffect(() => {
        const loadInitialProducts = async () => {
            setLoading(true);

            try {
                await loadProducts();
            } finally {
                setLoading(false);
            }
        };

        loadInitialProducts();
    }, [loadProducts]);


    useEffect(() => {
        const loadCategories = async () => {
            try {
                const { data } = await getCategories();
                setCategories(data);
            } catch (error) {
                console.error(error);
                toast.error(getErrorMessage(error));
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

        const payload = {
            name: product.name,
            description: product.description || null,
            imageUrl: product.imageUrl,
            price: Number(product.price)
        };

        try {
            if (product.id <= 0) {
                await createProduct(payload);
                await loadProducts();
            }
            else {
                await updateProduct(product.id, payload);

                setProducts((previousProducts) =>
                    previousProducts.map(p =>
                        p.id === product.id
                            ? { ...p, ...payload }
                            : p
                    )
                );
            }

            handleFormReset();
            toast.success("Saved successfully!");
        } catch (error) {
            console.error(error);
            toast.error(getErrorMessage(error));
        }
        finally {
            setLoading(false);
        }
    }

    const handleProductEdit = (product) => {
        setEditData(product);
    }

    // DELETE  /api/products/{id}
    const handleProductDelete = async (product) => {
        if (!confirm(`Are you sure to delete a product : ${product.name} ${product.description ?? ''}`)) {
            return;
        }

        setLoading(true);

        try {
            await deleteProduct(product.id);

            setProducts((previousProducts) =>
                previousProducts.filter(p => p.id !== product.id)
            );

            toast.success("Deleted successfully!");
        } catch (error) {
            console.error(error);
            toast.error(getErrorMessage(error));
        }
        finally {
            setLoading(false);
        }
    }

    // POST /api/products/{id}/stock/increase | /decrease  (body: { quantity })
    const handleStockChange = async (productId, quantity, action) => {
        setLoading(true);

        try {
            if (action === "increase") {
                await increaseStock(productId, quantity);
            } else {
                await decreaseStock(productId, quantity);
            }

            await loadProducts();

            toast.success(
                `Stock successfully ${action === "increase" ? "increased" : "decreased"}!`
            );
        } catch (error) {
            console.error(error);
            toast.error(getErrorMessage(error));
        }
        finally {
            setLoading(false);
        }
    }

    // POST /api/products/{id}/activate | /deactivate
    const handleToggleAvailability = async (product) => {
        setLoading(true);

        const action = product.isAvailable
            ? "deactivate"
            : "activate";

        try {
            if (action === "activate") {
                await activateProduct(product.id);
            } else {
                await deactivateProduct(product.id);
            }

            setProducts((previousProducts) =>
                previousProducts.map(p =>
                    p.id === product.id
                        ? { ...p, isAvailable: !product.isAvailable }
                        : p
                )
            );

            toast.success(`Product successfully ${action}d!`);
        } catch (error) {
            console.error(error);
            toast.error(getErrorMessage(error));
        }
        finally {
            setLoading(false);
        }
    }

    // POST /api/products/{productId}/categories/{categoryId}
    const handleAssignCategory = async (productId, categoryId) => {
        setLoading(true);

        try {
            await assignCategory(productId, categoryId);
            await loadProducts();

            toast.success("Category successfully assigned!");
        } catch (error) {
            console.error(error);
            toast.error(getErrorMessage(error));
        }
        finally {
            setLoading(false);
        }
    }

    // DELETE /api/products/{productId}/categories/{categoryId}
    const handleRemoveCategory = async (productId, categoryId) => {
        setLoading(true);

        try {
            await removeCategory(productId, categoryId);
            await loadProducts();

            toast.success("Category successfully removed!");
        } catch (error) {
            console.error(error);
            toast.error(getErrorMessage(error));
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