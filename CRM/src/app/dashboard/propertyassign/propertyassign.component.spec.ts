import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyassignComponent } from './propertyassign.component';

describe('PropertyassignComponent', () => {
  let component: PropertyassignComponent;
  let fixture: ComponentFixture<PropertyassignComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PropertyassignComponent]
    });
    fixture = TestBed.createComponent(PropertyassignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
