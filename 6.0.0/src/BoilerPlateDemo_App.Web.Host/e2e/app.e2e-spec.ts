import { BoilerPlateDemo_AppTemplatePage } from './app.po';

describe('BoilerPlateDemo_App App', function() {
  let page: BoilerPlateDemo_AppTemplatePage;

  beforeEach(() => {
    page = new BoilerPlateDemo_AppTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
