import { Injectable } from '@angular/core';
import { User } from './user.model';
import { LocalStorageService } from '../local-storage/local-storage.service';

@Injectable({
  providedIn: 'root',
})
export class UserAccessService {
  get user(): User | null {
    return this.localStorageService.getLocalStorage().user ?? null;
  }

  constructor(private readonly localStorageService: LocalStorageService) {}

  setUser(user: User) {
    const storage = this.localStorageService.getLocalStorage();

    this.localStorageService.setLocalStorage({
      ...storage,
      user,
    });
  }

  clearUser() {
    const storage = this.localStorageService.getLocalStorage();

    this.localStorageService.setLocalStorage({ ...storage, user: undefined });
  }
}
