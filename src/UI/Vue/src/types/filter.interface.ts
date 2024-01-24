export interface Filter {
    Logic?: string | null;
    Filters?: Filter[] | null;
    Field?: string | null;
    Operator?: string | null;
    Value?: any | null;
  }