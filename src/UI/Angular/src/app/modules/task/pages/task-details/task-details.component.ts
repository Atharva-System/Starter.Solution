import { Component, inject } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { ActivatedRoute } from '@angular/router';
import { ITaskDetails } from '../../models/task-details.interface';

@Component({
  selector: 'app-task-details',
  standalone: true,
  imports: [],
  templateUrl: './task-details.component.html',
  styleUrl: './task-details.component.css',
})
export class TaskDetailsComponent {
  taskService = inject(TaskService);
  route = inject(ActivatedRoute);
  taskId = '';
  taskDetails!: ITaskDetails;

  constructor() {
    this.route.params.subscribe((params) => {
      this.taskId = params['taskId'] || '';
      if (this.taskId) {
        this.setTaskDetails();
      }
    });
  }

  setTaskDetails() {
    this.taskService.getTask(this.taskId).subscribe((data) => {
      this.taskDetails = data.data;
    });
  }
}
