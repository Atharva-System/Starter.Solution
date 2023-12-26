import { Injectable } from '@angular/core';
import { PaginationFilter } from '../models/pagination-filter.interface';

@Injectable({
  providedIn: 'root',
})
export class CommonFilterService {
  defaultFilter: PaginationFilter = {
    AdvancedFilter: null,
    PageNumber: 1,
    PageSize: 10,
    OrderBy: [],
  };

  generateFilter(
    filters: any[],
    originalFilter: PaginationFilter,
  ): PaginationFilter {
    let allValuesUndefinedOrBlank = true;
    originalFilter.AdvancedFilter = {
      Filters: [],
      Logic: 'and',
    };
    filters.forEach((filter) => {
      if (
        filter.condition == 'is_not_null' ||
        filter.condition == 'is_null' ||
        (filter.value !== undefined &&
          filter.value !== null &&
          filter.value !== '')
      ) {
        allValuesUndefinedOrBlank = false;
        if (
          originalFilter &&
          originalFilter.AdvancedFilter &&
          originalFilter.AdvancedFilter.Filters
        )
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
}
