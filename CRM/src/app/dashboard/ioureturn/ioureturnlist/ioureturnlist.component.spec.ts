import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IoureturnlistComponent } from './ioureturnlist.component';

describe('IoureturnlistComponent', () => {
  let component: IoureturnlistComponent;
  let fixture: ComponentFixture<IoureturnlistComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [IoureturnlistComponent]
    });
    fixture = TestBed.createComponent(IoureturnlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
