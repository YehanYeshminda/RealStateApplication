import { Directive, ElementRef, HostListener, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { getDateOffset } from '../models/helpers';

@Directive({
  selector: '[dateTimeControls]'
})
export class DateTimeControlsDirective {
  @Input('dateTimeControls') formGroup!: FormGroup;
  @Input() controlName!: string;

  constructor(private el: ElementRef) { }

  @HostListener('click', ['$event.target'])
  onClick() {
    const increaseButton = this.el.nativeElement.classList;
    const decreaseButton = this.el.nativeElement.classList;

    if (increaseButton.contains('increase')) {
      this.increaseDate();
    } else if (decreaseButton.contains('decrease')) {
      this.decreaseDate();
    }
  }

  increaseDate() {
    const noteDate = this.formGroup.get(this.controlName)?.value;
    const newDate = getDateOffset(noteDate, 1);
    this.formGroup.get(this.controlName)?.setValue(newDate);
  }

  decreaseDate() {
    const noteDate = this.formGroup.get(this.controlName)?.value;
    const newDate = getDateOffset(noteDate, -1);
    this.formGroup.get(this.controlName)?.setValue(newDate);
  }
}
