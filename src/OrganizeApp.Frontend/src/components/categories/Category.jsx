import { useEffect, useState } from "react"
import CategoryForm from "./CategoryForm"
import CategoryList from "./CategoryList"
import { useForm } from "react-hook-form"
import toast from "react-hot-toast"
import {
    getCategories,
    createCategory,
    updateCategory,
    deleteCategory
} from "../../services/categoryService"
import { getErrorMessage } from "../../utils/errorHelper"

const defaultFormValues = {
    id: 0,
    name: '',
    description: '',
    code: ''
}

function Category() {
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
        const loadCategories = async () => {
            try {
                const { data } = await getCategories();
                setCategories(data);
            } catch (error) {
                console.error(error);
                toast.error(getErrorMessage(error));
            } finally {
                setLoading(false);
            }
        };

        loadCategories();
    }, []);

    const handleFormReset = () => {
        methods.reset(defaultFormValues);
    }

    const handleFormSubmit = async (category) => {
        setLoading(true);

        try {
            if (category.id <= 0) {
                const { data } = await createCategory(category);

                setCategories((previousCategories) => [
                    ...previousCategories,
                    data
                ]);
            }
            else {
                const { data } = await updateCategory(category.id, category);

                setCategories((previousCategories) =>
                    previousCategories.map(p =>
                        p.id === category.id ? data : p
                    )
                );
            }

            methods.reset(defaultFormValues);
            setEditData(null);
            toast.success("Saved successfully!");
        } catch (error) {
            console.error(error);
            toast.error(getErrorMessage(error));
        } finally {
            setLoading(false);
        }
    }

    const handleCategoryEdit = (category) => {
        setEditData(category);
    }

    const handleCategoryDelete = async (category) => {
        if (!confirm(`Are you sure to delete a category : ${category.name} ${category.description}`)) {
            return;
        }

        setLoading(true);

        try {
            await deleteCategory(category.id);

            setCategories((previousCategories) =>
                previousCategories.filter(p => p.id !== category.id)
            );

            toast.success("Deleted successfully!");
        } catch (error) {
            console.error(error);
            toast.error(getErrorMessage(error));
        } finally {
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

                <CategoryForm
                    methods={methods}
                    onFormSubmit={handleFormSubmit}
                    onFormReset={handleFormReset}
                />

                <CategoryList
                    categoriesList={categories}
                    onCategoryEdit={handleCategoryEdit}
                    onCategoryDelete={handleCategoryDelete}
                />
            </div>
        </div>
    )
}

export default Category