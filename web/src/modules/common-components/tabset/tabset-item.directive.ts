import { Directive, Input, TemplateRef } from '@angular/core';

@Directive({
  selector: '[bipTabItem]',
})
export class TabsetItemDirective {
  @Input() bipTabItem!: string;

  constructor(public readonly templateRef: TemplateRef<any>) {}
}
