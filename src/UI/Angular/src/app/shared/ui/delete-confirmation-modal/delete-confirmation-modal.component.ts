import {
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { ModalComponent } from '../modal/modal.component';

@Component({
  selector: 'app-delete-confirmation-modal',
  standalone: true,
  imports: [ModalComponent],
  templateUrl: './delete-confirmation-modal.component.html',
  styleUrl: './delete-confirmation-modal.component.css',
})
export class DeleteConfirmationModalComponent {
  @ViewChild('isDeleteModal') isDeleteModal!: ModalComponent;
  @Input() Title = 'Delete';
  @Input() Message = 'Are you sure you want to delete?';
  @Output() onDelete = new EventEmitter();
  @Output() onCancel = new EventEmitter();

  open() {
    setTimeout(() => {
      this.isDeleteModal.open();
    }, 10);
  }

  close() {
    this.isDeleteModal.close();
    this.onCancel.emit();
  }

  delete() {
    this.onDelete.emit();
  }
}
