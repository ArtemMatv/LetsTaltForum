import { Component, OnInit } from '@angular/core';
import { NavbarFooterService } from '../_services/nav-service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
    ngOnInit(): void {
        this.nav.show()
    }

    constructor(private nav: NavbarFooterService) {

    }
}
