import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertydevelopmentlistComponent } from './propertydevelopmentlist.component';

describe('PropertydevelopmentlistComponent', () => {
  let component: PropertydevelopmentlistComponent;
  let fixture: ComponentFixture<PropertydevelopmentlistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PropertydevelopmentlistComponent]
    });
    fixture = TestBed.createComponent(PropertydevelopmentlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
