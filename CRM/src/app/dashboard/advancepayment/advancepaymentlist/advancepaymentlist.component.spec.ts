import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvancepaymentlistComponent } from './advancepaymentlist.component';

describe('AdvancepaymentlistComponent', () => {
  let component: AdvancepaymentlistComponent;
  let fixture: ComponentFixture<AdvancepaymentlistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AdvancepaymentlistComponent]
    });
    fixture = TestBed.createComponent(AdvancepaymentlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
