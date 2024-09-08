import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CallCenterModalsComponent } from './call-center-modals.component';

describe('CallCenterModalsComponent', () => {
  let component: CallCenterModalsComponent;
  let fixture: ComponentFixture<CallCenterModalsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CallCenterModalsComponent]
    });
    fixture = TestBed.createComponent(CallCenterModalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
