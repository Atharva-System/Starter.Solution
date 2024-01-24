import { Filter } from './filter.interface';
import { Search } from './search.interface';

export interface BaseFilter {
  AdvancedSearch?: Search | null;
  Keyword?: string | null;
  AdvancedFilter?: Filter | null;
}
