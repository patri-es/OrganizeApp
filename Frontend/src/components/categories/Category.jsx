import { useEffect, useState } from "react"
import CategoryForm from "./CategoryForm"
import CategoryList from "./CategoryList"
import { useForm } from "react-hook-form"
import toast from "react-hot-toast"
import axios from "axios"

const BASE_URL = `${import.meta.env.VITE_BASE_API_URL}/categories`;

const defaultFormValues = {
    id: 0,
    name: '',
    description: '',
    code: ''
}
    // ,
    // category: ''

function Category() {
    //const BASE_URL = import.meta.env.VITE_BASE_API_URL + '/categories';

    const [categories, setCategories] = useState([]);
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
            const loadCategories = async () => {
                var categoriesData = (await axios.get(BASE_URL)).data;
                setCategories(categoriesData);
            }
            loadCategories();
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


    const handleFormSubmit = async (category) => {
        setLoading(true);
        try {
            if (category.id <= 0) {
                const createdCategory = (await axios.post(BASE_URL, category)).data;
                setCategories((previousCategory) => [...previousCategory, createdCategory]);
            }
            else {
                await axios.put(`${BASE_URL}/${category.id}`, category);
                setCategories((previousCategories) => previousCategories.map(p => p.id === category.id ? category : p));
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

    const handleCategoryEdit = (category) => {
        setEditData(category);
    }

    const handleCategoryDelete = async (category) => {
        if (!confirm(`Are you sure to delete a category : ${category.name} ${category.description}`)) return;
        setLoading(true);
        try {
            await axios.delete(`${BASE_URL}/${category.id}`);
            setCategories((previousCategory) => previousCategory.filter(p => p.id !== category.id));
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
                        Category Management
                    </h1>
                    {loading && <p>Loading...</p>}
                </div>

                <CategoryForm methods={methods} onFormSubmit={handleFormSubmit} onFormReset={handleFormReset} />
                <CategoryList categoriesList={categories} onCategoryEdit={handleCategoryEdit} onCategoryDelete={handleCategoryDelete} />
            </div>
        </div>
    )
}

export default Category