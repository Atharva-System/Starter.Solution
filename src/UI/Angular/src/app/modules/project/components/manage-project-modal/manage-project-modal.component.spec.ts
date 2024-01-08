import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageProjectModalComponent } from './manage-project-modal.component';

describe('ManageProjectModalComponent', () => {
  let component: ManageProjectModalComponent;
  let fixture: ComponentFixture<ManageProjectModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageProjectModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ManageProjectModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
