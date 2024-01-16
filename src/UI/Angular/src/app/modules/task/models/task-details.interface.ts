export interface ITaskDetails {
  id: string;
  taskName?: string;
  description?: string;
  startDate: string;
  endDate: string;
  status: number;
  priority: number;
  statusName?: string;
  priorityName?: string;
  projectId: string;
  assignedTo?: string;
  assignedToName?: string;
}
