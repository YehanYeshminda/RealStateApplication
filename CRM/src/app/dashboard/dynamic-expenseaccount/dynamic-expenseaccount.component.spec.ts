import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicExpenseaccountComponent } from './dynamic-expenseaccount.component';

describe('DynamicExpenseaccountComponent', () => {
  let component: DynamicExpenseaccountComponent;
  let fixture: ComponentFixture<DynamicExpenseaccountComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DynamicExpenseaccountComponent]
    });
    fixture = TestBed.createComponent(DynamicExpenseaccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
