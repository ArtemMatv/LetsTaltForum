import { Component, OnInit } from '@angular/core';
import { TopicService } from '../../_services/topic-service';
import { NavbarFooterService } from '../../_services/nav-service';

@Component({
    selector: 'app-topics',
    templateUrl: './topic-list.component.html',
    styleUrls: ['./topic-list.component.css']
})
export class TopicListComponent implements OnInit{
    ngOnInit(): void {
        this.nav.show()
    }
    public topics: ITopic[]
    private total: number;
    private page: number;
    private limit: number;

    public ableToGoNextPage(): boolean{
        return this.page*this.limit < this.total;
    }
    public ableToGoPreviousPage() : boolean{
        return this.page > 1;
    }

    constructor(private topicService: TopicService,
        private nav: NavbarFooterService) {
        this.page = 1;
        this.limit = 10;
        this.total = this.topicService.getTotal()
        this.getTopics()
    }

    getTopics(): void {
        this.topics = this.topicService.getTopics(this.page, this.limit)
    }

    goToPrevious(): void {
        //console.log('Previous Button Clicked!');
        if (this.ableToGoPreviousPage()) {
            this.page--;
            this.getTopics()
        }
    }

    goToNext(): void {
        //console.log('Next Button Clicked!');
        if (this.ableToGoNextPage()) {
            this.page++;
            this.getTopics()
        }
    }

}
