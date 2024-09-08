import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CallinsightComponent } from './CallinsightComponent';

describe('CallinsightComponent', () => {
  let component: CallinsightComponent;
  let fixture: ComponentFixture<CallinsightComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CallinsightComponent]
    });
    fixture = TestBed.createComponent(CallinsightComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
