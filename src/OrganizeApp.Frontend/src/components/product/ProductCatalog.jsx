import { useEffect, useMemo, useState } from "react";
import toast from "react-hot-toast";

import { getProducts } from "../../services/productService";
import { getCategories } from "../../services/categoryService";
import { getErrorMessage } from "../../utils/errorHelper";

const initialFilters = {
    categoryIds: [],
    onlyAvailable: false,
    onlyInStock: false
};

function ProductCatalog({ children }) {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [filters, setFilters] = useState(initialFilters);
    const [sortPrice, setSortPrice] = useState("");
    const [loading, setLoading] = useState(true);

    // Cargar categorías.
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

    // Cargar productos cuando cambien los filtros.
    useEffect(() => {
        const loadProducts = async () => {
            setLoading(true);

            try {
                const params = {};

                if (filters.categoryIds.length > 0) {
                    params.CategoryIds = filters.categoryIds;
                }

                if (filters.onlyAvailable) {
                    params.IsAvailable = true;
                }

                if (filters.onlyInStock) {
                    params.MinStock = 1;
                }

                const { data } = await getProducts(params);
                setProducts(data);
            } catch (error) {
                console.error(error);
                toast.error(getErrorMessage(error));
            } finally {
                setLoading(false);
            }
        };

        loadProducts();
    }, [filters]);

    // Ordenar los productos sin modificar la lista original.
    const sortedProducts = useMemo(() => {
        if (!sortPrice) {
            return products;
        }

        return [...products].sort((a, b) =>
            sortPrice === "asc"
                ? a.price - b.price
                : b.price - a.price
        );
    }, [products, sortPrice]);

    return children({
        products: sortedProducts,
        categories,
        filters,
        onFiltersChange: setFilters,
        sortPrice,
        onSortPriceChange: setSortPrice,
        loading
    });
}

export default ProductCatalog;