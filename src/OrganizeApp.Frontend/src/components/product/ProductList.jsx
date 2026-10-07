import { useState } from 'react'
import { Edit, Trash2, Plus, Minus, Tag, X, Power, PowerOff } from 'lucide-react'

const ProductList = ({
    productsList,
    categories = [],
    onProductEdit,
    onProductDelete,
    onStockChange,
    onToggleAvailability,
    onAssignCategory,
    onRemoveCategory
}) => {

    const [stockQuantities, setStockQuantities] = useState({});
    const [selectedCategories, setSelectedCategories] = useState({});

    if (!productsList || productsList.length === 0) {
        return (
            <div className="text-center text-gray-500">
                <p className="text-lg">
                    No tenemos productos en la lista
                </p>
            </div>
        )
    }

    const getStockQuantity = (productId) => stockQuantities[productId] ?? 1;

    const setStockQuantity = (productId, value) => {
        setStockQuantities(prev => ({ ...prev, [productId]: value }));
    };

    const getSelectedCategory = (productId) => selectedCategories[productId] ?? '';

    const setSelectedCategory = (productId, value) => {
        setSelectedCategories(prev => ({ ...prev, [productId]: value }));
    };

    const handleAssign = (productId) => {
        const categoryId = Number(getSelectedCategory(productId));
        if (!categoryId) return;
        onAssignCategory(productId, categoryId);
    };

    return (
        <div className="bg-white rounded-xl shadow-lg border border-gray-100 overflow-hidden">
            <ul className="divide-y divide-gray-100">
                {
                    productsList.map(product => (
                        <li key={product.id} className="p-4 space-y-3">
                            {/* Datos del producto */}
                            <div className="flex items-center justify-between">
                                <div>
                                    <span className="font-semibold text-gray-800">{product.name} </span>
                                    <span className="text-gray-500">{product.description} </span>
                                    <span className="ml-2 text-sm font-medium text-gray-700">
                                        {product.price?.toFixed(2)} €
                                    </span>
                                    <span className={`ml-2 text-sm ${product.stock > 0 ? 'text-green-600' : 'text-red-600'}`}>
                                        Stock: {product.stock}
                                    </span>
                                    <span className={`ml-2 inline-flex items-center rounded-full px-2 py-0.5 text-xs font-medium ${product.isAvailable ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-500'}`}>
                                        {product.isAvailable ? 'Available' : 'Unavailable'}
                                    </span>
                                </div>
                                <div className="flex items-center space-x-2">
                                    <button onClick={() => onProductEdit(product)}
                                        className="inline-flex items-center px-3 py-2 bg-blue-100 text-blue-700 text-sm font-medium rounded-lg hover:bg-blue-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-1 transition-all duration-200 transform hover:scale-105"
                                        title="Edit product">
                                        <Edit className="w-4 h-4 mr-1" />
                                        Edit
                                    </button>
                                    <button onClick={() => onProductDelete(product)}
                                        className="inline-flex items-center px-3 py-2 bg-red-100 text-red-700 text-sm font-medium rounded-lg hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-1 transition-all duration-200 transform hover:scale-105"
                                        title="Delete product">
                                        <Trash2 className="w-4 h-4 mr-1" />
                                        Delete
                                    </button>
                                    <button onClick={() => onToggleAvailability(product)}
                                        className={`inline-flex items-center px-3 py-2 text-sm font-medium rounded-lg focus:outline-none focus:ring-2 focus:ring-offset-1 transition-all duration-200 transform hover:scale-105 ${product.isAvailable
                                            ? 'bg-amber-100 text-amber-700 hover:bg-amber-200 focus:ring-amber-500'
                                            : 'bg-green-100 text-green-700 hover:bg-green-200 focus:ring-green-500'}`}
                                        title={product.isAvailable ? 'Deactivate product' : 'Activate product'}>
                                        {product.isAvailable
                                            ? <><PowerOff className="w-4 h-4 mr-1" /> Deactivate</>
                                            : <><Power className="w-4 h-4 mr-1" /> Activate</>}
                                    </button>
                                </div>
                            </div>

                            {/* Control de stock: POST /{id}/stock/increase|decrease */}
                            <div className="flex items-center space-x-2">
                                <span className="text-sm font-semibold text-gray-600">Stock:</span>
                                <input
                                    type="number" min="1"
                                    value={getStockQuantity(product.id)}
                                    onChange={(e) => setStockQuantity(product.id, Number(e.target.value))}
                                    className="w-20 px-2 py-1.5 rounded-lg border border-gray-200 text-gray-700 text-sm focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500"
                                />
                                <button
                                    onClick={() => onStockChange(product.id, getStockQuantity(product.id), 'increase')}
                                    disabled={getStockQuantity(product.id) <= 0}
                                    className="inline-flex items-center px-3 py-2 bg-green-100 text-green-700 text-sm font-medium rounded-lg hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-1 transition-all duration-200 transform hover:scale-105 disabled:opacity-50"
                                    title="Increase stock">
                                    <Plus className="w-4 h-4 mr-1" />
                                    Add
                                </button>
                                <button
                                    onClick={() => onStockChange(product.id, getStockQuantity(product.id), 'decrease')}
                                    disabled={getStockQuantity(product.id) <= 0 || getStockQuantity(product.id) > product.stock}
                                    className="inline-flex items-center px-3 py-2 bg-orange-100 text-orange-700 text-sm font-medium rounded-lg hover:bg-orange-200 focus:outline-none focus:ring-2 focus:ring-orange-500 focus:ring-offset-1 transition-all duration-200 transform hover:scale-105 disabled:opacity-50"
                                    title="Decrease stock">
                                    <Minus className="w-4 h-4 mr-1" />
                                    Remove
                                </button>
                            </div>

                            {/* Categorías: POST/DELETE /{productId}/categories/{categoryId} */}
                            <div className="flex flex-wrap items-center gap-2">
                                <span className="text-sm font-semibold text-gray-600">Categories:</span>
                                {product.categories?.map(category => (
                                    <span key={category.id}
                                        className="inline-flex items-center rounded-full bg-teal-50 px-3 py-1 text-xs font-medium text-teal-700">
                                        <Tag className="w-3 h-3 mr-1" />
                                        {category.name}
                                        <button
                                            onClick={() => onRemoveCategory(product.id, category.id)}
                                            className="ml-1 text-teal-500 hover:text-red-600 transition-colors"
                                            title={`Remove category ${category.name}`}>
                                            <X className="w-3 h-3" />
                                        </button>
                                    </span>
                                ))}
                                <select
                                    value={getSelectedCategory(product.id)}
                                    onChange={(e) => setSelectedCategory(product.id, e.target.value)}
                                    className="px-2 py-1.5 rounded-lg border border-gray-200 text-sm text-gray-700 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500">
                                    <option value="">Select category...</option>
                                    {categories
                                        .filter(c => !product.categories?.some(pc => pc.id === c.id))
                                        .map(category => (
                                            <option key={category.id} value={category.id}>
                                                {category.name}
                                            </option>
                                        ))}
                                </select>
                                <button
                                    onClick={() => handleAssign(product.id)}
                                    disabled={!getSelectedCategory(product.id)}
                                    className="inline-flex items-center px-3 py-2 bg-purple-100 text-purple-700 text-sm font-medium rounded-lg hover:bg-purple-200 focus:outline-none focus:ring-2 focus:ring-purple-500 focus:ring-offset-1 transition-all duration-200 transform hover:scale-105 disabled:opacity-50"
                                    title="Assign category">
                                    <Plus className="w-4 h-4 mr-1" />
                                    Assign
                                </button>
                            </div>
                        </li>
                    ))
                }
            </ul>
        </div>
    )
}

export default ProductList