import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvancepaymentComponent } from './advancepayment.component';

describe('AdvancepaymentComponent', () => {
  let component: AdvancepaymentComponent;
  let fixture: ComponentFixture<AdvancepaymentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdvancepaymentComponent]
    });
    fixture = TestBed.createComponent(AdvancepaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
