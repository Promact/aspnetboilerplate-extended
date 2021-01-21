import { AspnetBoilerplateExtendedTemplatePage } from './app.po';

describe('AspnetBoilerplateExtended App', function() {
  let page: AspnetBoilerplateExtendedTemplatePage;

  beforeEach(() => {
    page = new AspnetBoilerplateExtendedTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
