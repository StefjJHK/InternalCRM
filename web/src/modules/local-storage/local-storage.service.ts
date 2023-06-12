import { Injectable } from '@angular/core';
import { LocalStorage } from './local-storage.model';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  private static readonly localStorageKey = 'storage';

  getLocalStorage(): LocalStorage {
    const storage = localStorage.getItem(LocalStorageService.localStorageKey);

    if (!storage) {
      this.setLocalStorage({});

      return {};
    }

    return JSON.parse(storage);
  }

  setLocalStorage(storage: LocalStorage) {
    localStorage.setItem(LocalStorageService.localStorageKey, JSON.stringify(storage));
  }
}
