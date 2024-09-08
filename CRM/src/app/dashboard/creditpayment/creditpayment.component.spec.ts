import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreditpaymentComponent } from './creditpayment.component';

describe('CreditpaymentComponent', () => {
  let component: CreditpaymentComponent;
  let fixture: ComponentFixture<CreditpaymentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreditpaymentComponent]
    });
    fixture = TestBed.createComponent(CreditpaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
