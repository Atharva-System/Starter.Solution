import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaveOrDiscardChangesModalComponent } from './save-or-discard-changes-modal.component';

describe('SaveOrDiscardChangesModalComponent', () => {
  let component: SaveOrDiscardChangesModalComponent;
  let fixture: ComponentFixture<SaveOrDiscardChangesModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SaveOrDiscardChangesModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SaveOrDiscardChangesModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
