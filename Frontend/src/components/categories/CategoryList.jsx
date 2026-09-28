import { Edit, Trash2 } from 'lucide-react'

const CategoryList = ({ categoriesList, onCategoryEdit, onCategoryDelete }) => {

    if (!categoriesList || categoriesList.length === 0) {
        return (
            <div className="text-center text-gray-500">
                <p className="text-lg">
                    No tenemos categorías en la lista
                </p>
            </div>
        )
    }

    return (
        <div>
            <ul>
                {
                    categoriesList.map(category => <li key={category.id}
                        className="">
                        <div>
                            <span>{category.name} </span>
                            <span>{category.description} </span>
                        </div>
                        <div className="flex items-center justify-center space-x-2">
                            <button onClick={() => onCategoryEdit(category)}
                                className="inline-flex items-center px-3 py-2 bg-blue-100 text-blue-700 text-sm font-medium rounded-lg hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200 transform hover:scale-105"
                                title="Edit category"
                            >
                                <Edit className="w-4 h-4 mr-1" />
                                Edit
                            </button>
                            <button
                                onClick={() => onCategoryDelete(category)}

                                className="inline-flex items-center px-3 py-2 bg-red-100 text-red-700 text-sm font-medium rounded-lg hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200 transform hover:scale-105"
                                title="Delete category"
                            >
                                <Trash2 className="w-4 h-4 mr-1" />
                                Delete
                            </button>

                        </div>
                    </li>

                    )
                }



            </ul>
        </div>

    )
}

export default CategoryList