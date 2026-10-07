import { Save, RotateCcw } from 'lucide-react';

// CreateProductDTO / UpdateProductDTO: name (req, máx 100), description (opc, máx 500),
// imageUrl (req al crear, máx 500), price (req, >= 0).
// El stock NO se envía: empieza en 0 y se gestiona con los endpoints increase/decrease.
const ProductForm = ({ methods, onFormReset, onFormSubmit, isEditing }) => {

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = methods;

    return (
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
            <form
                className="bg-white rounded-xl shadow-lg border border-gray-100 overflow-hidden"
                onSubmit={handleSubmit(onFormSubmit)}>
                <input type="hidden" {...register("id")} id="id" name="id" />

                <div className="px-6 py-5 border-b border-gray-100">
                    <h3 className="text-xl font-bold bg-gradient-to-r from-blue-600 to-teal-600 bg-clip-text text-transparent">
                        {isEditing ? 'Edit Product' : 'Add Product'}
                    </h3>
                    <div className="h-1 w-16 mt-3 rounded-full bg-gradient-to-r from-blue-600 to-teal-500"></div>
                </div>

                <div className="p-6 grid grid-cols-1 md:grid-cols-2 gap-5">
                    {/* Name */}
                    <div>
                        <label htmlFor="name" className="block text-sm font-semibold text-gray-700 mb-2">
                            Name
                        </label>
                        <input type="text" placeholder="Enter name"
                            {...register("name", {
                                required: true,
                                maxLength: 100
                            })}
                            className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />

                        {errors.name?.type === 'required' &&
                            <p className="mt-1 text-sm text-red-600 flex items-center">
                                name is required
                            </p>}
                        {errors.name?.type === 'maxLength' &&
                            <p className="mt-1 text-sm text-red-600 flex items-center">
                                name can not exceed 100 characters
                            </p>}
                    </div>

                    {/* Description */}
                    <div>
                        <label htmlFor="description" className="block text-sm font-semibold text-gray-700 mb-2">
                            Description
                        </label>
                        <input type="text" placeholder="Enter description"
                            {...register("description", {
                                required: false,
                                maxLength: 500
                            })}
                            className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />

                        {errors.description?.type === 'maxLength' &&
                            <p className="mt-1 text-sm text-red-600 flex items-center">
                                description can not exceed 500 characters
                            </p>}
                    </div>

                    {/* imageUrl */}
                    <div>
                        <label htmlFor="imageUrl" className="block text-sm font-semibold text-gray-700 mb-2">
                            Image URL
                        </label>
                        <input type="text" placeholder="Enter image URL"
                            {...register("imageUrl", {
                                required: true,
                                maxLength: 500
                            })}
                            className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />

                        {errors.imageUrl?.type === 'required' &&
                            <p className="mt-1 text-sm text-red-600 flex items-center">
                                image URL is required
                            </p>}
                        {errors.imageUrl?.type === 'maxLength' &&
                            <p className="mt-1 text-sm text-red-600 flex items-center">
                                image URL can not exceed 500 characters
                            </p>}
                    </div>

                    {/* Price */}
                    <div>
                        <label htmlFor="price" className="block text-sm font-semibold text-gray-700 mb-2">
                            Price
                        </label>
                        <input type="number" step="0.01" min="0" placeholder="Enter price"
                            {...register("price", {
                                required: true,
                                min: 0
                            })}
                            className="w-full px-4 py-3 rounded-lg border border-gray-200 text-gray-700 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-teal-500 focus:border-teal-500 transition-all duration-200" />

                        {errors.price?.type === 'required' &&
                            <p className="mt-1 text-sm text-red-600 flex items-center">
                                price is required
                            </p>}
                        {errors.price?.type === 'min' &&
                            <p className="mt-1 text-sm text-red-600 flex items-center">
                                price can not be negative
                            </p>}
                    </div>
                </div>

                {/* footer con botones */}
                <div className="px-6 py-4 bg-gray-50 border-t border-gray-100 flex justify-end">
                    <button type="submit"
                        className="px-4 py-2.5 rounded-full text-sm font-semibold text-white bg-gradient-to-b from-teal-500 via-teal-600 to-teal-500 hover:from-teal-600 hover:to-blue-600 shadow-md hover:shadow-lg transition-all duration-300 transform hover:scale-105"
                        title="Save product">
                        <Save />
                    </button>
                    <button type="button"
                        onClick={onFormReset}
                        className="ms-3 px-4 py-2.5 rounded-full text-sm font-semibold text-white bg-gradient-to-b from-blue-400 via-blue-600 to-blue-400 hover:from-teal-600 hover:to-blue-600 shadow-md hover:shadow-lg transition-all duration-300 transform hover:scale-105"
                        title="Reset form">
                        <RotateCcw />
                    </button>
                </div>
            </form>
        </div>
    );
}

export default ProductForm