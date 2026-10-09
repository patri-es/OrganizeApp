function ProductFilters({
    categories = [],
    filters,
    onFiltersChange,
    layout = "horizontal"
}) {
    const isSidebar = layout === "sidebar";

    const containerClasses = isSidebar
        ? "flex flex-col gap-5"
        : "flex flex-wrap items-center gap-6";

    const categoryClasses = isSidebar
        ? "w-full"
        : "w-full sm:w-56";

    const handleCategoryChange = (e) => {
        onFiltersChange({
            ...filters,
            categoryId: e.target.value
        });
    };

    const handleAvailabilityChange = (e) => {
        onFiltersChange({
            ...filters,
            onlyAvailable: e.target.checked
        });
    };

    const handleStockChange = (e) => {
        onFiltersChange({
            ...filters,
            onlyInStock: e.target.checked
        });
    };

    const handleReset = () => {
        onFiltersChange({
            categoryId: "",
            onlyAvailable: false,
            onlyInStock: false
        });
    };

    return (
        <div className="w-full rounded-lg border border-gray-200 bg-white p-4 shadow-sm">
            <div className={containerClasses}>
                {/* Categoría */}
                <div className={categoryClasses}>
                    <label
                        htmlFor="product-category-filter"
                        className="mb-1 block text-sm font-medium text-gray-700"
                    >
                        Categoría
                    </label>

                    <select
                        id="product-category-filter"
                        value={filters.categoryId}
                        onChange={handleCategoryChange}
                        className="w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm text-gray-700 focus:border-teal-500 focus:outline-none focus:ring-2 focus:ring-teal-500"
                    >
                        <option value="">Todas las categorías</option>

                        {categories.map(category => (
                            <option
                                key={category.id}
                                value={category.id}
                            >
                                {category.name}
                            </option>
                        ))}
                    </select>
                </div>

                {/* Disponibilidad */}
                <label className="flex items-center gap-2 text-sm text-gray-700">
                    <input
                        type="checkbox"
                        checked={filters.onlyAvailable}
                        onChange={handleAvailabilityChange}
                        className="h-4 w-4 rounded border-gray-300 accent-teal-600"
                    />
                    Solo disponibles
                </label>

                {/* Stock */}
                <label className="flex items-center gap-2 text-sm text-gray-700">
                    <input
                        type="checkbox"
                        checked={filters.onlyInStock}
                        onChange={handleStockChange}
                        className="h-4 w-4 rounded border-gray-300 accent-teal-600"
                    />
                    Solo con stock
                </label>

                {/* Restablecer */}
                <button
                    type="button"
                    onClick={handleReset}
                    className="text-left text-sm font-medium text-teal-700 hover:text-teal-900"
                >
                    Limpiar filtros
                </button>
            </div>
        </div>
    );
}

export default ProductFilters;