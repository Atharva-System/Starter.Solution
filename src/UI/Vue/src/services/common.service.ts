import { PaginationFilter } from '@/types/pagination-filter.interface';

class CommonService {
    defaultFilter = {
        AdvancedFilter: null,
        PageNumber: 1,
        PageSize: 10,
        OrderBy: [],
    } as PaginationFilter;

    generateFilter(filters: any[], originalFilter: PaginationFilter): PaginationFilter {
        let allValuesUndefinedOrBlank = true;
        originalFilter.AdvancedFilter = {
            Filters: [],
            Logic: 'and',
        };
        filters.forEach((filter) => {
            if (
                filter.condition === 'is_not_null' ||
                filter.condition === 'is_null' ||
                (filter.value !== undefined && filter.value !== null && filter.value !== '')
            ) {
                allValuesUndefinedOrBlank = false;
                if (originalFilter && originalFilter.AdvancedFilter && originalFilter.AdvancedFilter.Filters)
                    originalFilter.AdvancedFilter.Filters.push({
                        Field: filter.field,
                        Operator: filter.condition,
                        Value: filter.value,
                    });
            }
        });
        if (allValuesUndefinedOrBlank) {
            originalFilter.AdvancedFilter = null;
        }
        return originalFilter;
    }

    generateSingleFilter(condition: string, field: string, value: any, originalFilter: PaginationFilter): PaginationFilter {
        originalFilter.AdvancedFilter = {
            Filters: [],
            Logic: 'and',
        };
        if (condition && field && value) {
            if (originalFilter && originalFilter.AdvancedFilter && originalFilter.AdvancedFilter.Filters)
                originalFilter.AdvancedFilter.Filters.push({
                    Field: field,
                    Operator: condition,
                    Value: value,
                });
        } else {
            originalFilter.AdvancedFilter = null;
        }
        return originalFilter;
    }

    dateRangeSingleFilter(fieldDates: any, valueDates: any, originalFilter: PaginationFilter): PaginationFilter {
        originalFilter.AdvancedFilter = {
            Filters: [],
            Logic: 'and',
        };
        if (fieldDates && valueDates) {
            if (originalFilter && originalFilter.AdvancedFilter && originalFilter.AdvancedFilter.Filters)
                originalFilter.AdvancedFilter.Filters.push(
                    {
                        Field: fieldDates.start,
                        Operator: 'gte',
                        Value: valueDates.start,
                    },
                    {
                        Field: fieldDates.to,
                        Operator: 'lte',
                        Value: valueDates.to,
                    },
                );
        } else {
            originalFilter.AdvancedFilter = null;
        }
        return originalFilter;
    }
}
export default new CommonService();
