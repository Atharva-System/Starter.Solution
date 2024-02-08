import { PaginationFilter } from "./types/pagination-filter.interface";

class FilterService {
    defaultFilter(): PaginationFilter {
        return {
            AdvancedFilter: null,
            PageNumber: 1,
            PageSize: 10,
            OrderBy: [],
        } as PaginationFilter;
    }
}
export default new FilterService();
