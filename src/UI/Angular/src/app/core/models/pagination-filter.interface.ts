import { BaseFilter } from './base-filter.interface';

export interface PaginationFilter extends BaseFilter {
  PageNumber: number;
  PageSize: number;
  OrderBy?: string[] | null;
}
