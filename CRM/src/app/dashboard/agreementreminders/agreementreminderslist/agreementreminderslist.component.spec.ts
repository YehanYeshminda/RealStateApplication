import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgreementreminderslistComponent } from './agreementreminderslist.component';

describe('AgreementreminderslistComponent', () => {
  let component: AgreementreminderslistComponent;
  let fixture: ComponentFixture<AgreementreminderslistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AgreementreminderslistComponent]
    });
    fixture = TestBed.createComponent(AgreementreminderslistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
