import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InviteUserModalComponent } from './invite-user-modal.component';

describe('InviteUserModalComponent', () => {
  let component: InviteUserModalComponent;
  let fixture: ComponentFixture<InviteUserModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InviteUserModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InviteUserModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
