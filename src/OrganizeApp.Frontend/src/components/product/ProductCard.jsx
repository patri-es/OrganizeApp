function ProductCard({ product }) {
    return (
        <article className="flex h-full flex-col overflow-hidden rounded-lg border border-gray-200 bg-white shadow-sm transition-shadow hover:shadow-md">
            {/* Imagen */}
            <div className="aspect-square w-full overflow-hidden bg-gray-100">
                {product.imageUrl ? (
                    <img
                        src={product.imageUrl}
                        alt={product.name}
                        className="h-full w-full object-contain p-4"
                    />
                ) : (
                    <div className="flex h-full items-center justify-center text-sm text-gray-400">
                        Sin imagen
                    </div>
                )}
            </div>

            {/* Contenido */}
            <div className="flex flex-1 flex-col p-4">
                {/* Nombre */}
                <h2 className="line-clamp-2 text-lg font-semibold text-gray-900">
                    {product.name}
                </h2>

                {/* Descripción */}
                {product.description && (
                    <p className="mt-2 line-clamp-3 text-sm text-gray-600">
                        {product.description}
                    </p>
                )}

                {/* Categorías */}
                {product.categories?.length > 0 && (
                    <div className="mt-3 flex flex-wrap gap-1">
                        {product.categories.map((category) => (
                            <span
                                key={category.id}
                                className="rounded-full bg-gray-100 px-2 py-1 text-xs text-gray-600"
                            >
                                {category.name}
                            </span>
                        ))}
                    </div>
                )}

                {/* Precio */}
                <div className="mt-4">
                    <span className="text-2xl font-medium text-gray-900">
                        {(product.price ?? 0).toFixed(2)} €
                    </span>
                </div>

                {/* Stock */}
                <div className="mt-2 text-sm">
                    {product.stock > 0 ? (
                        <span className="text-green-600">
                            {product.stock} unidades disponibles
                        </span>
                    ) : (
                        <span className="text-red-600">
                            Agotado
                        </span>
                    )}
                </div>

                {/* Disponibilidad */}
                <div className="mt-1 text-sm">
                    <span className={product.isAvailable ? 'text-green-600' : 'text-gray-400'}>
                        {product.isAvailable ? 'Disponible' : 'No disponible'}
                    </span>
                </div>
            </div>
        </article>
    )
}

export default ProductCard