import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreditpaymentlistComponent } from './creditpaymentlist.component';

describe('CreditpaymentlistComponent', () => {
  let component: CreditpaymentlistComponent;
  let fixture: ComponentFixture<CreditpaymentlistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CreditpaymentlistComponent]
    });
    fixture = TestBed.createComponent(CreditpaymentlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
