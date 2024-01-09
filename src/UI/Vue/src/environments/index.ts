import prod from './environment';
import dev from './environment.dev';
import docker from './environment.docker';

const env = process.env.NODE_ENV == 'development' ? dev : (process.env.NODE_ENV == 'docker' ? docker : prod);
export default env;