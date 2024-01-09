export interface ICreateTask {
  taskName?: string;
  description?: string;
  startDate: Date;
  endDate: Date;
  status: number;
  priority: number;
  projectId: string;
  assignedTo?: string;
}
