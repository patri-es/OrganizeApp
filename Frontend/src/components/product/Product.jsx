import { useEffect, useState } from "react"
import ProductForm from "./ProductForm"
import ProductList from "./ProductList"
import { useForm } from "react-hook-form"
import toast from "react-hot-toast"
import axios from "axios"

const BASE_URL = `${import.meta.env.VITE_BASE_API_URL}/products`;

const defaultFormValues = {
    id: 0,
    name: '',
    description: '',
    creationDate: '',
    category: ''
}

function Product() {
    //const BASE_URL = import.meta.env.VITE_BASE_API_URL + '/products';

    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [editData, setEditData] = useState(null);

    const methods = useForm({
        defaultValues: defaultFormValues
    });

    useEffect(() => {
        methods.reset(editData ?? defaultFormValues);
    }, [editData, methods]);

    useEffect(() => {
        try {
            const loadProducts = async () => {
                var productsData = (await axios.get(BASE_URL)).data;
                setProducts(productsData);
            }
            loadProducts();
        } catch (error) {
            console.log(error);
            toast.error("Error has occured!");
        }
        finally {
            setLoading(false);
        }

    }, []);

    // useEffect(() => {
    //     methods.reset(editData);
    // }, [editData])

    // reset
    const handleFormReset = () => {
        methods.reset(defaultFormValues);
    }


    const handleFormSubmit = async (product) => {
        setLoading(true);
        try {
            if (product.id <= 0) {
                const createdProduct = (await axios.post(BASE_URL, product)).data;
                setProducts((previousProduct) => [...previousProduct, createdProduct]);
            }
            else {
                await axios.put(`${BASE_URL}/${product.id}`, product);
                setProducts((previousProducts) => previousProducts.map(p => p.id === product.id ? product : p));
            }
            methods.reset(defaultFormValues);
            toast.success("Saved successfully!");
        } catch (error) {
            console.log(error);
            toast.error("Error has occured!");
        }
        finally {
            setLoading(false);
        }
    }

    const handleProductEdit = (product) => {
        setEditData(product);
    }

    const handleProductDelete = async (product) => {
        if (!confirm(`Are you sure to delete a product : ${product.name} ${product.description}`)) return;
        setLoading(true);
        try {
            await axios.delete(`${BASE_URL}/${product.id}`);
            setProducts((previousProduct) => previousProduct.filter(p => p.id !== product.id));
            toast.success("Deleted successfully!");
        } catch (error) {
            console.log(error);
            toast.error("Error on deleting!");
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

                <ProductForm methods={methods} onFormSubmit={handleFormSubmit} onFormReset={handleFormReset} />
                <ProductList productsList={products} onProductEdit={handleProductEdit} onProductDelete={handleProductDelete} />
            </div>
        </div>
    )
}

export default Product