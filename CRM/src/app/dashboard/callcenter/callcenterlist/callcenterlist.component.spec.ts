import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CallcenterlistComponent } from './callcenterlist.component';

describe('CallcenterlistComponent', () => {
  let component: CallcenterlistComponent;
  let fixture: ComponentFixture<CallcenterlistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CallcenterlistComponent]
    });
    fixture = TestBed.createComponent(CallcenterlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
