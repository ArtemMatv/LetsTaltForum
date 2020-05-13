import { Component } from '@angular/core';
import { NavbarFooterService } from '../_services/nav-service';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
})
export class FooterComponent {

    constructor(private nav: NavbarFooterService) {

    }
}
