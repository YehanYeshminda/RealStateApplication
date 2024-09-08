import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeadsforwardlistComponent } from './leadsforwardlist.component';

describe('LeadsforwardlistComponent', () => {
  let component: LeadsforwardlistComponent;
  let fixture: ComponentFixture<LeadsforwardlistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LeadsforwardlistComponent]
    });
    fixture = TestBed.createComponent(LeadsforwardlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
