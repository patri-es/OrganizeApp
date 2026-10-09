import { useEffect, useState } from "react";
import toast from "react-hot-toast";

import { getProducts } from "../../services/productService";
import { getCategories } from "../../services/categoryService";
import { getErrorMessage } from "../../utils/errorHelper";

import ProductFilters from "./ProductFilters";
import ProductGrid from "./ProductGrid";

const initialFilters = {
    categoryId: "",
    onlyAvailable: false,
    onlyInStock: false
};

function ProductCatalog({ filterLayout = "horizontal" }) {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [filters, setFilters] = useState(initialFilters);
    const [loading, setLoading] = useState(true);

    // Cargar las categorías disponibles.
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

    // Cargar los productos cada vez que cambien los filtros.
    useEffect(() => {
        const loadProducts = async () => {
            setLoading(true);

            try {
                const params = {};

                if (filters.categoryId !== "") {
                    params.CategoryId = Number(filters.categoryId);
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

    return (
        <section className="w-full space-y-6">
            <ProductFilters
                categories={categories}
                filters={filters}
                onFiltersChange={setFilters}
                layout={filterLayout}
            />

            <ProductGrid
                products={products}
                loading={loading}
            />
        </section>
    );
}

export default ProductCatalog;