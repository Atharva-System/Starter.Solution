export interface IUpdateProject {
  id: string;
  projectName?: string;
  description?: string;
  startDate: Date;
  endDate: Date;
  estimatedHours: string;
}
