import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CallCenterApproveModalComponent } from './call-center-approve-modal.component';

describe('CallCenterApproveModalComponent', () => {
  let component: CallCenterApproveModalComponent;
  let fixture: ComponentFixture<CallCenterApproveModalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CallCenterApproveModalComponent]
    });
    fixture = TestBed.createComponent(CallCenterApproveModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
