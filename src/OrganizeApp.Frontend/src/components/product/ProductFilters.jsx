function ProductFilters({
    categories = [],
    filters,
    onFiltersChange,
    sortPrice,
    onSortPriceChange,
    layout = "horizontal"
}) {
    const isSidebar = layout === "sidebar";

    const containerClasses = isSidebar
        ? "flex flex-col gap-5"
        : "flex flex-wrap items-end gap-6";

    const categoryClasses = isSidebar
        ? "w-full"
        : "w-full sm:w-64";

    const addCategory = (e) => {
        const categoryId = Number(e.target.value);

        if (!categoryId || filters.categoryIds.includes(categoryId)) {
            return;
        }

        onFiltersChange({
            ...filters,
            categoryIds: [...filters.categoryIds, categoryId]
        });
    };

    const removeCategory = (categoryId) => {
        onFiltersChange({
            ...filters,
            categoryIds: filters.categoryIds.filter(id => id !== categoryId)
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
            categoryIds: [],
            onlyAvailable: false,
            onlyInStock: false
        });

        onSortPriceChange("");
    };

    const selectedCategories = categories.filter(category =>
        filters.categoryIds.includes(category.id)
    );

    const availableCategories = categories.filter(category =>
        !filters.categoryIds.includes(category.id)
    );

    return (
        <div className="w-full rounded-lg border border-gray-200 bg-white p-4 shadow-sm">
            <div className={containerClasses}>
                {/* Categorías */}
                <div className={categoryClasses}>
                    <label
                        htmlFor="product-category-filter"
                        className="mb-1 block text-sm font-medium text-gray-700"
                    >
                        Categorías
                    </label>

                    <select
                        id="product-category-filter"
                        value=""
                        onChange={addCategory}
                        className="w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm text-gray-700 focus:border-teal-500 focus:outline-none focus:ring-2 focus:ring-teal-500"
                    >
                        <option value="" disabled>
                            Seleccionar categoría...
                        </option>

                        {availableCategories.map(category => (
                            <option key={category.id} value={category.id}>
                                {category.name}
                            </option>
                        ))}
                    </select>

                    {/* Categorías seleccionadas */}
                    {selectedCategories.length > 0 && (
                        <div className="mt-3 flex flex-wrap gap-2">
                            {selectedCategories.map(category => (
                                <span
                                    key={category.id}
                                    className="inline-flex items-center rounded-full bg-purple-100 px-2 py-1 text-xs font-medium text-purple-700"
                                >
                                    {category.name}

                                    <button
                                        type="button"
                                        onClick={() => removeCategory(category.id)}
                                        className="ml-1 text-purple-500 transition-colors hover:text-red-600"
                                        title={`Quitar categoría ${category.name}`}
                                        aria-label={`Quitar categoría ${category.name}`}
                                    >
                                        ×
                                    </button>
                                </span>
                            ))}
                        </div>
                    )}
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

                {/* Ordenación por precio */}
                <div className={categoryClasses}>
                    <label
                        htmlFor="product-price-sort"
                        className="mb-1 block text-sm font-medium text-gray-700"
                    >
                        Ordenar por precio
                    </label>

                    <select
                        id="product-price-sort"
                        value={sortPrice}
                        onChange={(e) => onSortPriceChange(e.target.value)}
                        className="w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm text-gray-700 focus:border-teal-500 focus:outline-none focus:ring-2 focus:ring-teal-500"
                    >
                        <option value="">Orden predeterminado</option>
                        <option value="asc">Precio: menor a mayor</option>
                        <option value="desc">Precio: mayor a menor</option>
                    </select>
                </div>

                {/* Limpiar filtros */}
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