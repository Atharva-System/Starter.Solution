import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageTaskModalComponent } from './manage-task-modal.component';

describe('ManageTaskModalComponent', () => {
  let component: ManageTaskModalComponent;
  let fixture: ComponentFixture<ManageTaskModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageTaskModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ManageTaskModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
