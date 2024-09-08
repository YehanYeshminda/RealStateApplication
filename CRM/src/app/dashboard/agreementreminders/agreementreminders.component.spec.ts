import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgreementremindersComponent } from './agreementreminders.component';

describe('AgreementremindersComponent', () => {
  let component: AgreementremindersComponent;
  let fixture: ComponentFixture<AgreementremindersComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AgreementremindersComponent]
    });
    fixture = TestBed.createComponent(AgreementremindersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
