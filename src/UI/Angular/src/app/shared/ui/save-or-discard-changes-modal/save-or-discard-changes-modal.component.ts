import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { ModalComponent } from '../modal/modal.component';
import { ButtonComponent } from '../button/button.component';

@Component({
  selector: 'app-save-or-discard-changes-modal',
  standalone: true,
  imports: [ModalComponent, ButtonComponent],
  templateUrl: './save-or-discard-changes-modal.component.html',
  styleUrl: './save-or-discard-changes-modal.component.css',
})
export class SaveOrDiscardChangesModalComponent {
  @ViewChild('saveOrDiscardChangesModal')
  saveOrDiscardChangesModal!: ModalComponent;
  @Output() saveChanges = new EventEmitter();
  @Output() discardChanges = new EventEmitter();

  save() {
    this.saveChanges.emit();
    this.closeModal();
  }

  discard() {
    this.discardChanges.emit();
    this.closeModal();
  }

  openModal() {
    this.saveOrDiscardChangesModal.open();
  }

  closeModal() {
    this.saveOrDiscardChangesModal.close();
  }
}
