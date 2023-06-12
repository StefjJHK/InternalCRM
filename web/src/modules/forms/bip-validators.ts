import { AbstractControl, ValidationErrors } from '@angular/forms';
import emailValidator from 'email-validator';
import { isPossiblePhoneNumber } from 'libphonenumber-js';

export class BipValidators {
  public static required(): (control: AbstractControl) => ValidationErrors | null {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (!value) {
        return { required: { message: `Is mandatory` } };
      }

      return null;
    };
  }

  public static phoneNumber(): (control: AbstractControl) => ValidationErrors | null {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (!isPossiblePhoneNumber(value)) {
        return { phoneNumber: { message: `Must be phone number` } };
      }

      return null;
    };
  }

  public static email(): (control: AbstractControl) => ValidationErrors | null {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (!emailValidator.validate(value)) {
        return { email: { message: `Must be email` } };
      }

      return null;
    };
  }

  public static mustBe(validationString: string): (control: AbstractControl) => ValidationErrors | null {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;

      if (value != validationString) {
        return { validationString: { message: `Must be ${validationString}` } };
      }

      return null;
    };
  }
}
